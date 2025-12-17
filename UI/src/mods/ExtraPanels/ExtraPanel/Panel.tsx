import React, {
    forwardRef,
    useMemo,
    useContext,
    ReactNode,
} from "react";
import classNames from "classnames"

/* =========================================================
 * 🔧 OBFUSCATED DEPENDENCIES (TO REPLACE LATER)
 * ======================================================= */

// Focus key factory (ex: new Og("PanelHeader"))
const createFocusKey = (name: string) => ({ name });

// Theme resolver (YE(theme))
const resolveTheme = (theme: PanelTheme) => theme;

// Detect TV / Gamepad mode (Km())
const useIsTVMode = (): boolean => false;

// Play transition sounds (bp)
const playTransitionSounds = (sounds?: unknown) => { };

// Focus + action stubs
const ActionHandler = (props: any) => <>{props.children}</>; // --> "game-ui/common/input-events/input-action-consumer.tsx"
const FocusTrap = ({ children }: any) => <>{children}</>;
const TransitionWrapper = ({ children }: any) => <>{children}</>; // --> ""game-ui/common/animations/class-name-transition.tsx""
const FocusPanel = forwardRef<HTMLDivElement, any>((props, ref) => ( // --> "game-ui/common/focus/focus-div.tsx"
    <div ref={ref} {...props} /> 
));
const FocusGroup = ({ children }: any) => <>{children}</>; // --> "game-ui/common/focus/auto-navigation-scope.tsx"
const FocusItem = ({ children }: any) => <>{children}</>; // --> ""game-ui/common/focus/focus-key-override.tsx""
const Hint = (_props: any) => null; // --> "game-ui/common/input-events/action-hints/input-hint/floating-input-hint.tsx"

// Action resolution (Vm)
const hasAction = (_opts: { action: string; context?: any }) => false; // --> ""game-ui/common/data-binding/input-bindings.ts""

// Focus state detection (kh)
const useHasFocus = (_ctx: any) => true; // --> "game-ui/common/focus/focus-hooks.tsx"

// Context stub
const FocusContext = React.createContext<any>(null); // --> "game-ui/common/focus/controller/focus-controller.ts"

/* =========================================================
 * 🎨 TYPES
 * ======================================================= */

export interface PanelTheme {
    panel?: string;
    header?: string;
    content?: string;
    footer?: string;
}

export interface PanelProps extends React.HTMLAttributes<HTMLDivElement> {
    focusKey?: any;

    header?: ReactNode;
    footer?: ReactNode;

    theme?: PanelTheme;
    transition?: any;
    transitionSounds?: unknown;

    contentClassName?: string;

    onClose?: () => void;
    allowFocusExit?: boolean;
    allowLooping?: boolean;

    showCloseHint?: boolean | string;
    hintClassName?: string;
    unfocusedHintAction?: string;
    footerHintAsTooltip?: boolean;

    backActionOverride?: string;
    actionContext?: any;
}

/* =========================================================
 * 🔑 FOCUS KEYS
 * ======================================================= */

const PANEL_HEADER_KEY = createFocusKey("PanelHeader");
const PANEL_CONTENT_KEY = createFocusKey("PanelContent");
const PANEL_FOOTER_KEY = createFocusKey("PanelFooter");

/* =========================================================
 * 🧠 CONTEXT
 * ======================================================= */

interface PanelContextValue {
    theme: PanelTheme;
    onClose?: () => void;
}

const PanelContext = React.createContext<PanelContextValue | null>(null);

/* =========================================================
 * 📦 PANEL COMPONENT
 * ======================================================= */

export const Panel = forwardRef<HTMLDivElement, PanelProps>(function Panel(
    {
        focusKey,
        header,
        footer,
        theme = {},
        transition,
        transitionSounds,

        className,
        contentClassName,
        children,

        onClose,
        allowFocusExit = false,
        allowLooping = false,

        showCloseHint,
        hintClassName,
        unfocusedHintAction,
        footerHintAsTooltip,

        backActionOverride,
        actionContext,

        ...rest
    },
    ref
) {
    /* -----------------------------
     * Setup
     * ---------------------------- */

    playTransitionSounds(transitionSounds);

    const resolvedTheme = resolveTheme(theme);
    const isTVMode = useIsTVMode();

    const hasHeader = React.Children.count(header) > 0;
    const hasContent = React.Children.count(children) > 0;
    const hasFooter = React.Children.count(footer) > 0;

    const contextValue = useMemo<PanelContextValue>(
        () => ({
            theme: resolvedTheme,
            onClose,
        }),
        [resolvedTheme, onClose]
    );

    /* -----------------------------
     * UI
     * ---------------------------- */

    const panelUI = (
        <ActionHandler
            onAction={onClose}
            action={backActionOverride}
            actionContext={actionContext}
        >
            <PanelContext.Provider value={contextValue}>
                <TransitionWrapper styles={transition}>
                    <FocusPanel
                        ref={ref}
                        focusKey={focusKey}
                        className={resolvedTheme.panel || className}
                        {...rest}
                    >
                        <FocusGroup
                            initialFocused={PANEL_CONTENT_KEY}
                            allowLooping={allowLooping}
                            direction="vertical"
                        >
                            {hasHeader && (
                                <FocusItem focusKey={PANEL_HEADER_KEY}>
                                    <div className={resolvedTheme.header}>{header}</div>
                                </FocusItem>
                            )}

                            {hasContent && (
                                <FocusItem focusKey={PANEL_CONTENT_KEY}>
                                    <div className={contentClassName || resolvedTheme.content}>
                                        {children}
                                    </div>
                                </FocusItem>
                            )}

                            {hasFooter && (
                                <FocusItem focusKey={PANEL_FOOTER_KEY}>
                                    <div className={resolvedTheme.footer}>{footer}</div>
                                </FocusItem>
                            )}
                        </FocusGroup>

                        {isTVMode && (showCloseHint || unfocusedHintAction) && (
                            <PanelHint
                                focusedAction={
                                    typeof showCloseHint === "boolean"
                                        ? undefined
                                        : showCloseHint
                                }
                                unfocusedAction={unfocusedHintAction}
                                asTooltip={footerHintAsTooltip}
                                actionContext={actionContext}
                                className={hintClassName} // --> + "game-ui/common/panel/themes/panel-transition.module.scss".floatingHint
                            />
                        )}
                    </FocusPanel>
                </TransitionWrapper>
            </PanelContext.Provider>
        </ActionHandler>
    );

    return allowFocusExit ? panelUI : <FocusTrap>{panelUI}</FocusTrap>;
});

/* =========================================================
 * 💡 PANEL HINT
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
    const hasFocus = useHasFocus(useContext(FocusContext));

    const canClose = hasAction({ action: "Close", context: props.actionContext });
    const canBack = hasAction({ action: "Back", context: props.actionContext });

    if (hasFocus || (unfocusedAction && !asTooltip)) {
        return (
            <Hint
                {...props}
                action={
                    focusedAction ??
                    (canClose ? "Close" : canBack ? "Back" : undefined)
                }
                active={!!focusedAction}
            />
        );
    }

    if (unfocusedAction && asTooltip) {
        return (
            <Hint
                {...props}
                action={unfocusedAction}
                tooltip // --> "game-ui/common/panel/themes/panel-transition.module.scss".tooltipHint
                active
            />
        );
    }

    return null;
};
