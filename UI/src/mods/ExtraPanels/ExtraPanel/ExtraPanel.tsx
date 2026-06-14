import { Panel, ResizeResult } from "./Panel"
import { ExtraPanelType, SetExpandedExtraPanel, SetFullScreenExtraPanel, SetPanelPosition, SetPanelSize } from "../ExtraPanelType"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import { useState } from "react"
import classNames from "classnames"
import ExtraPanelSCSS from "./ExtraPanel.module.scss"
import { BetterDragEventData, BetterDragHandle } from "../../Utilities/BetterDragHandle"
import { Number2 } from "cs2/bindings"

export interface propsExtraPanel {
    extraPanel: ExtraPanelType,
    children: any,
    zIndex?: number,
    onBringToFront?: () => void,
}

const ROOT_FONT_SIZE_VH = parseFloat(getComputedStyle(document.documentElement).fontSize);
const getRemInPx = () => (ROOT_FONT_SIZE_VH / 100) * window.innerHeight;
const pxToRem = (px: number) => px / getRemInPx();
const remToPx = (rem: number) => rem * getRemInPx();

export const ExtraPanel = ({ extraPanel, children, zIndex, onBringToFront }: propsExtraPanel) => {

    const [translate, setTranslate] = useState({ x: 0, y: 0 }); 

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
        setTranslate({ x: 0.0, y: 0.0 });
    }

    return <Panel
        onMouseDown={onBringToFront}
        header={BetterDragHandle({ onDragStart: onDragStart, onDrag: onDrag, onDragEnd: onDragEnd, children: ExtraPanelHeader({ extraPanel }) })}
        // footer={"FOOTER"}
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
        {extraPanel.isExpanded && children}
    </Panel>
}
