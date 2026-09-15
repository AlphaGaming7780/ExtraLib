import { Panel, ResizeResult } from "./Panel"
import { ExtraPanelType, SetExpandedExtraPanel, SetFullScreenExtraPanel, SetPanelPosition, SetPanelSize } from "../ExtraPanelType"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import { ExtraFooterRenderer } from "./Footer/ExtraFooterRenderer"
import { memo, useMemo, useState } from "react"
import classNames from "classnames"
import ExtraPanelSCSS from "./ExtraPanel.module.scss"
import { BetterDragEventData, BetterDragHandle } from "../../Utilities/BetterDragHandle"
import { Number2 } from "cs2/bindings"
import { TypedRenderer } from "../../../../game-ui/common/typed-renderer/typed-renderer"
import { extraPanelsComponents, extraPanelsFooterComponents } from "../ExtraPanelEntryPoint"
import { pxToRem, remToPx } from "../../Utilities/RemHelper"

export interface propsExtraPanel {
    extraPanel: ExtraPanelType,
    zIndex?: number,
    onBringToFront?: () => void,
}


const MemoizedPanelContent = memo(
    ({ components, data, props }: { components: any; data: ExtraPanelType; props: ExtraPanelType }) =>
        <TypedRenderer components={components} data={data} props={props} />,
    (prev, next) =>
        prev.data.__Type === next.data.__Type &&
        prev.data.visible === next.data.visible &&
        prev.data.isExpanded === next.data.isExpanded &&
        prev.data.canFullScreen === next.data.canFullScreen &&
        prev.data.isFullScreen === next.data.isFullScreen &&
        prev.data.showInSelector === next.data.showInSelector &&
        prev.data.panelMinSize.x === next.data.panelMinSize.x &&
        prev.data.panelMinSize.y === next.data.panelMinSize.y &&
        prev.data.icon === next.data.icon
);

export const ExtraPanel = ({ extraPanel, zIndex, onBringToFront }: propsExtraPanel) => {

    const [translate, setTranslate] = useState({ x: 0, y: 0 });

    const [sizePreview, setSizePreview] = useState<Number2 | null>(null);

    const getTranslate = (
        { x, y, startX, startY }: BetterDragEventData
    ): Number2 => {

        let transX = x - startX;
        let transY = y - startY;

        let finalPanelX = extraPanel.panelLocation.x + transX;
        let finalPanelY = extraPanel.panelLocation.y + transY;

        var clampedPanelPosx = Math.min(Math.max(finalPanelX, 0), window.innerWidth);
        var clampedPanelPosy = Math.min(Math.max(finalPanelY, 0), window.innerHeight);

        const translateX = clampedPanelPosx - extraPanel.panelLocation.x;
        const translateY = clampedPanelPosy - extraPanel.panelLocation.y;

        return { x: translateX, y: translateY };
    };

    const onDragStart = (b: BetterDragEventData): boolean => {
        return !extraPanel.isFullScreen
    }

    const onDrag = (b: BetterDragEventData) => {
        extraPanel.isFullScreen && SetFullScreenExtraPanel(extraPanel, false)
        const { x, y } = getTranslate(b);
        setTranslate({ x: x, y: y })
    }

    const onDragEnd = (b: BetterDragEventData) => {
        const { x, y } = getTranslate(b);

        extraPanel.panelLocation = { x: extraPanel.panelLocation.x + x, y: extraPanel.panelLocation.y + y }
        setTranslate({ x: 0.0, y: 0.0 })
        SetPanelPosition(extraPanel, extraPanel.panelLocation);
    }

    const onResizing = ({ width, height, deltaX, deltaY }: ResizeResult) =>
    {

        if(deltaY !== 0 && !extraPanel.isExpanded)
        {
            extraPanel.isExpanded = true;
            SetExpandedExtraPanel(extraPanel, true);
        }

        setSizePreview({ x: pxToRem(width), y: pxToRem(height) });

        if (deltaX !== 0 || deltaY !== 0) {
            setTranslate({x: deltaX, y: deltaY});
        }
        else
        {
            setTranslate({ x: 0.0, y: 0.0 });
        }
    }

    const onResizeEnd = ({ width, height, deltaX, deltaY }: ResizeResult) =>
    {
        const widthRem = pxToRem(width);
        const heightRem = pxToRem(height);

        // console.log(`Resizing ended. New size: ${widthRem}rem x ${heightRem}rem, New size in px: ${width}px x ${height}px, Delta: ${deltaX}px x ${deltaY}px, font size: ${getRemInPx()}px, getComputedStyle font size: ${getComputedStyle(document.documentElement).fontSize}`);

        extraPanel.panelSize = { x: widthRem, y: heightRem }

        SetPanelSize(extraPanel, extraPanel.panelSize);

        if (deltaX !== 0 || deltaY !== 0) {
            const newPos = {
                x: extraPanel.panelLocation.x + deltaX,
                y: extraPanel.panelLocation.y + deltaY,
            };
            extraPanel.panelLocation = newPos;
            SetPanelPosition(extraPanel, extraPanel.panelLocation);
        }
        setSizePreview(null);
        setTranslate({ x: 0.0, y: 0.0 });
    }

    const effectiveExtraPanel: ExtraPanelType = useMemo(() => ({
        ...extraPanel,
        panelSize: sizePreview ?? extraPanel.panelSize,
        panelLocation: (translate.x || translate.y)
            ? { x: extraPanel.panelLocation.x + translate.x, y: extraPanel.panelLocation.y + translate.y }
            : extraPanel.panelLocation,
    }), [extraPanel, sizePreview, translate]);

    const hasFooterComponent = extraPanel.__Type in extraPanelsFooterComponents;

    return <Panel
        onMouseDown={onBringToFront}
        header={BetterDragHandle({ onDragStart: onDragStart, onDrag: onDrag, onDragEnd: onDragEnd, children: ExtraPanelHeader({ extraPanel }) })}
        footer={hasFooterComponent ? <ExtraFooterRenderer extraPanel={effectiveExtraPanel} /> : undefined}
        className={classNames(
            "draggable-panel",
            ExtraPanelSCSS.ExtraPanel,
            extraPanel.isFullScreen && ExtraPanelSCSS.FullScreen,
            !extraPanel.isExpanded && ExtraPanelSCSS.Collapsed,
            !extraPanel.visible && ExtraPanelSCSS.Hidden,
        )}
        resizable={!extraPanel.isFullScreen}
        onResizing={onResizing}
        onResizeEnd={onResizeEnd}
        minWidth={remToPx(extraPanel.panelMinSize.x)}
        minHeight={remToPx(extraPanel.panelMinSize.y)}
        style={{
            left: extraPanel.panelLocation.x,
            top: extraPanel.panelLocation.y,
            width: extraPanel.panelSize.x == 0 ? "auto" : `${extraPanel.panelSize.x}rem`,
            height: extraPanel.panelSize.y == 0 ? "auto" : `${extraPanel.panelSize.y}rem`,
            zIndex: zIndex,
            transform: (translate.x || translate.y)
                ? `translate(${translate.x}px, ${translate.y}px)`
                : undefined,
        }}
    >
        {extraPanel.isExpanded && (
            <MemoizedPanelContent components={extraPanelsComponents} data={effectiveExtraPanel} props={effectiveExtraPanel} />
        )}
    </Panel>
}
