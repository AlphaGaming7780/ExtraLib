import React, {
    forwardRef,
    useMemo,
    useContext,
    useCallback,
    useRef,
    useState,
    ReactNode,
} from "react";
import classNames from "classnames";

import { InputActionConsumer } from "../../../../game-ui/common/input-events/input-action-consumer";
import { ClassNameTransition } from "../../../../game-ui/common/animations/class-name-transition";
import { useTransitionSounds } from "../../../../game-ui/common/animations/transition-sounds";
import { ActiveFocusDiv } from "../../../../game-ui/common/focus/focus-div";
import { AutoNavigationScope } from "../../../../game-ui/common/focus/auto-navigation-scope";
import { FocusKeyOverride } from "../../../../game-ui/common/focus/focus-key-override";
import { FocusRoot } from "../../../../game-ui/common/focus/focus-root";
import { useFocused } from "../../../../game-ui/common/focus/focus-hooks";
import { FocusContext } from "../../../../game-ui/common/focus/focus-controller";
import { NavigationDirection } from "../../../../game-ui/common/focus/navigation";
import { useGamepadActive } from "../../../../game-ui/common/hooks/use-control-scheme";
import { useInputActionActive } from "../../../../game-ui/common/data-binding/input-bindings";
import { FloatingInputHint } from "../../../../game-ui/common/input-events/action-hints/input-hint/floating-input-hint";
import { usePanelTheme } from "../../../../game-ui/common/panel/panel-theme";
import { PanelContext } from "../../../../game-ui/common/panel/panel-context";
import { DefaultPanelSCSS } from "../../../../game-ui/common/panel/themes/default.module.scss";
import { PanelTransitionSCSS } from "../../../../game-ui/common/panel/themes/panel-transition.module.scss";
import styles from "./ExtraPanel.module.scss";

/* =========================================================
 * TYPES
 * ======================================================= */

export interface PanelTheme {
    panel?: string;
    header?: string;
    content?: string;
    footer?: string;
    floatingHint?: string;
    tooltipHint?: string;
}

export interface ResizeEdge {
    top?: boolean;
    bottom?: boolean;
    left?: boolean;
    right?: boolean;
}

export interface PanelProps extends Omit<React.HTMLAttributes<HTMLDivElement>, "onResize"> {
    focusKey?: any;

    header?: ReactNode;
    footer?: ReactNode;

    theme?: PanelTheme;
    transition?: any;
    transitionSounds?: { enter?: string; exit?: string };

    contentClassName?: string;
    contentStyle?: React.CSSProperties;

    onClose?: () => void;
    allowFocusExit?: boolean;
    allowLooping?: boolean;

    showCloseHint?: boolean | string;
    hintClassName?: string;
    unfocusedHintAction?: string;
    footerHintAsTooltip?: boolean;

    backActionOverride?: string;
    actionContext?: any;

    resizable?: boolean | ResizeEdge;
    minWidth?: number;
    minHeight?: number;
    maxWidth?: number;
    maxHeight?: number;
    onResizing?: (result: ResizeResult) => void;
    onResizeEnd?: (result: ResizeResult) => void;
}

/* =========================================================
 * FOCUS KEYS
 * ======================================================= */

import { useUniqueFocusKey, FOCUS_AUTO$ } from "../../../../game-ui/common/focus/focus-key";

/* =========================================================
 * RESIZE HANDLE
 * ======================================================= */

type ResizeDirection = "n" | "s" | "e" | "w" | "ne" | "nw" | "se" | "sw";

const RESIZE_CLASSES: Record<ResizeDirection, string> = {
    n: styles.ResizeN,
    s: styles.ResizeS,
    e: styles.ResizeE,
    w: styles.ResizeW,
    ne: styles.ResizeNE,
    nw: styles.ResizeNW,
    se: styles.ResizeSE,
    sw: styles.ResizeSW,
};

interface ResizeHandleProps {
    direction: ResizeDirection;
    onResizeStart: (direction: ResizeDirection, e: React.MouseEvent) => void;
}

const ResizeHandle = ({ direction, onResizeStart }: ResizeHandleProps) => (
    <div
        className={RESIZE_CLASSES[direction]}
        onMouseDown={(e) => {
            e.preventDefault();
            e.stopPropagation();
            onResizeStart(direction, e);
        }}
    >
        <div className={styles.Indicator} />
    </div>
);

export interface ResizeResult {
    width: number;
    height: number;
    /** Delta X of the panel origin (non-zero when resizing from left/top edge) */
    deltaX: number;
    /** Delta Y of the panel origin (non-zero when resizing from top edge) */
    deltaY: number;
}

function useResize(
    ref: React.RefObject<HTMLDivElement | null>,
    opts: {
        enabled: boolean;
        edges: ResizeEdge;
        minWidth: number;
        minHeight: number;
        maxWidth: number;
        maxHeight: number;
        onResizing?: (result: ResizeResult) => void;
        onResizeEnd?: (result: ResizeResult) => void;
    }
) {
    const [sizeOverride, setSizeOverride] = useState<{ width: number; height: number } | null>(null);

    const handleResizeStart = useCallback(
        (direction: ResizeDirection, e: React.MouseEvent) => {
            if (!opts.enabled || !ref.current) return;
            const el = ref.current;
            const rect = el.getBoundingClientRect();
            const startX = e.clientX;
            const startY = e.clientY;
            const startW = rect.width;
            const startH = rect.height;

            const computeResize = (dx: number, dy: number): ResizeResult => {
                let newW = startW;
                let newH = startH;
                let deltaX = 0;
                let deltaY = 0;

                if (direction.includes("e")) newW = startW + dx;
                if (direction.includes("w")) { newW = startW - dx; }
                if (direction.includes("s")) newH = startH + dy;
                if (direction.includes("n")) { newH = startH - dy; }

                // Clamp
                const clampedW = Math.min(Math.max(newW, opts.minWidth), opts.maxWidth);
                const clampedH = Math.min(Math.max(newH, opts.minHeight), opts.maxHeight);

                // When resizing from left/top, the panel origin needs to move
                if (direction.includes("w")) deltaX = startW - clampedW;
                if (direction.includes("n")) deltaY = startH - clampedH;

                return { width: clampedW, height: clampedH, deltaX, deltaY };
            };

            const onMouseMove = (ev: MouseEvent) => {
                const dx = ev.clientX - startX;
                const dy = ev.clientY - startY;
                const result = computeResize(dx, dy);
                setSizeOverride({ width: result.width, height: result.height });
                opts.onResizing?.(result);
            };

            const onMouseUp = (ev: MouseEvent) => {
                document.removeEventListener("mousemove", onMouseMove);
                document.removeEventListener("mouseup", onMouseUp);

                const dx = ev.clientX - startX;
                const dy = ev.clientY - startY;
                const result = computeResize(dx, dy);
                setSizeOverride(null);
                opts.onResizeEnd?.(result);
            };

            document.addEventListener("mousemove", onMouseMove);
            document.addEventListener("mouseup", onMouseUp);
        },
        [opts.enabled, opts.minWidth, opts.minHeight, opts.maxWidth, opts.maxHeight, opts.onResizing, opts.onResizeEnd, ref]
    );

    const resizeHandles = useMemo(() => {
        if (!opts.enabled) return null;
        const edges = opts.edges;
        const handles: ResizeDirection[] = [];
        if (edges.top) handles.push("n");
        if (edges.bottom) handles.push("s");
        if (edges.left) handles.push("w");
        if (edges.right) handles.push("e");
        if (edges.top && edges.right) handles.push("ne");
        if (edges.top && edges.left) handles.push("nw");
        if (edges.bottom && edges.right) handles.push("se");
        if (edges.bottom && edges.left) handles.push("sw");
        return handles.map((dir) => (
            <ResizeHandle key={dir} direction={dir} onResizeStart={handleResizeStart} />
        ));
    }, [opts.enabled, opts.edges, handleResizeStart]);

    return { sizeOverride, resizeHandles };
}

/* =========================================================
 * PANEL COMPONENT
 * ======================================================= */

export const Panel = forwardRef<HTMLDivElement, PanelProps>(function Panel(
    {
        focusKey,
        header,
        footer,
        theme = DefaultPanelSCSS,
        transition = PanelTransitionSCSS,
        transitionSounds,

        className,
        contentClassName,
        contentStyle,
        children,
        style,

        onClose,
        allowFocusExit = false,
        allowLooping = false,

        showCloseHint,
        hintClassName,
        unfocusedHintAction,
        footerHintAsTooltip,

        backActionOverride,
        actionContext,

        resizable = false,
        minWidth = 165,
        minHeight = 68,
        maxWidth = Infinity,
        maxHeight = Infinity,
        onResizing,
        onResizeEnd,

        ...rest
    },
    ref
) {
    const internalRef = useRef<HTMLDivElement>(null);
    const panelRef = (ref as React.RefObject<HTMLDivElement>) || internalRef;

    const PANEL_HEADER_KEY = useUniqueFocusKey(FOCUS_AUTO$, "PanelHeader");
    const PANEL_CONTENT_KEY = useUniqueFocusKey(FOCUS_AUTO$, "PanelContent");
    const PANEL_FOOTER_KEY = useUniqueFocusKey(FOCUS_AUTO$, "PanelFooter");

    useTransitionSounds(transitionSounds);

    const resolvedTheme = usePanelTheme(theme);
    const isGamepad = useGamepadActive();

    const hasHeader = React.Children.count(header) > 0;
    const hasContent = React.Children.count(children) > 0;
    const hasFooter = React.Children.count(footer) > 0;

    const contextValue = useMemo(
        () => ({
            theme: resolvedTheme,
            onClose,
        }),
        [resolvedTheme, onClose]
    );

    const resizeEdges = useMemo<ResizeEdge>(() => {
        if (resizable === true) return { top: true, bottom: true, left: true, right: true };
        if (resizable === false) return {};
        return resizable;
    }, [resizable]);

    const { sizeOverride, resizeHandles } = useResize(panelRef, {
        enabled: !!resizable,
        edges: resizeEdges,
        minWidth,
        minHeight,
        maxWidth,
        maxHeight,
        onResizing,
        onResizeEnd,
    });

    const mergedStyle = useMemo<React.CSSProperties | undefined>(() => {
        if (!sizeOverride && !style) return undefined;
        return {
            ...style,
            ...(sizeOverride ? { width: sizeOverride.width, height: sizeOverride.height } : {}),
        };
    }, [sizeOverride, style]);

    const panelUI = (
        <InputActionConsumer
            onAction={onClose}
            action={backActionOverride}
            actionContext={actionContext}
        >
            <PanelContext.Provider value={contextValue}>
                <ClassNameTransition styles={transition}>
                    <ActiveFocusDiv
                        ref={panelRef}
                        focusKey={focusKey}
                        className={classNames(resolvedTheme.panel, className)}
                        style={mergedStyle}
                        {...rest}
                    >
                        <AutoNavigationScope
                            initialFocused={PANEL_CONTENT_KEY}
                            allowLooping={allowLooping}
                            direction={NavigationDirection.Vertical}
                        >
                            {hasHeader && (
                                <FocusKeyOverride focusKey={PANEL_HEADER_KEY}>
                                    <div className={resolvedTheme.header}>{header}</div>
                                </FocusKeyOverride>
                            )}

                            {hasContent && (
                                <FocusKeyOverride focusKey={PANEL_CONTENT_KEY}>
                                    <div className={classNames(resolvedTheme.content, contentClassName)} style={contentStyle}>
                                        {children}
                                    </div>
                                </FocusKeyOverride>
                            )}

                            {hasFooter && (
                                <FocusKeyOverride focusKey={PANEL_FOOTER_KEY}>
                                    <div className={resolvedTheme.footer}>{footer}</div>
                                </FocusKeyOverride>
                            )}
                        </AutoNavigationScope>

                        {isGamepad && (showCloseHint || unfocusedHintAction) && (
                            <PanelHint
                                asTooltip={footerHintAsTooltip}
                                actionContext={actionContext}
                                focusedAction={
                                    typeof showCloseHint === "boolean"
                                        ? undefined
                                        : showCloseHint
                                }
                                unfocusedAction={unfocusedHintAction}
                                className={classNames(DefaultPanelSCSS.floatingHint, hintClassName)}
                            />
                        )}

                        {resizeHandles}
                    </ActiveFocusDiv>
                </ClassNameTransition>
            </PanelContext.Provider>
        </InputActionConsumer>
    );

    return allowFocusExit ? panelUI : <FocusRoot>{panelUI}</FocusRoot>;
});

/* =========================================================
 * PANEL HINT
 * ======================================================= */

interface PanelHintProps {
    focusedAction?: string;
    unfocusedAction?: string;
    asTooltip?: boolean;
    className?: string;
    actionContext?: any;
}

const PanelHint = ({
    focusedAction,
    unfocusedAction,
    asTooltip,
    ...props
}: PanelHintProps) => {
    const hasFocus = useFocused(useContext(FocusContext));

    const canClose = useInputActionActive({ action: "Close", context: props.actionContext });
    const canBack = useInputActionActive({ action: "Back", context: props.actionContext });

    if (hasFocus || (unfocusedAction && !asTooltip)) {
        return (
            <FloatingInputHint
                {...props}
                action={
                    focusedAction || canClose ? "Close" : canBack ? "Back" : undefined
                }
                active={!!focusedAction}
            />
        );
    }

    if (unfocusedAction && asTooltip) {
        return (
            <FloatingInputHint
                {...props}
                action={unfocusedAction}
                tooltipClassName={DefaultPanelSCSS.tooltipHint}
                tooltip
                active
            />
        );
    }

    return null;
};
