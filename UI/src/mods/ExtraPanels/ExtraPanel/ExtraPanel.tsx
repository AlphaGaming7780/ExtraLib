import { Panel, ResizeResult } from "./Panel"
import { ExtraPanelType, SetFullScreenExtraPanel, SetPanelPosition, SetPanelSize } from "../ExtraPanelType"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import { useState } from "react"
import { DragHandle } from "../../../../game-ui/common/input/drag-handle"
import classNames from "classnames"
import ExtraPanelSCSS from "./ExtraPanel.module.scss"
import { BetterDragEventData, BetterDragHandle, BetterDragHandleProps } from "../../Utilities/BetterDragHandle"
import { Number2 } from "cs2/bindings"

export interface propsExtraPanel {
    extraPanel: ExtraPanelType,
    children: any
}

export const ExtraPanel = ({ extraPanel, children }: propsExtraPanel) => {

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
        if (deltaX !== 0 || deltaY !== 0) {
            setTranslate({x: deltaX, y: deltaY});
        }
    }

    const onResizeEnd = ({ width, height, deltaX, deltaY }: ResizeResult) => 
    {
        extraPanel.panelSize = { x: width, y: height }
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
        header={BetterDragHandle({ onDragStart: onDragStart, onDrag: onDrag, onDragEnd: onDragEnd, children: ExtraPanelHeader({ extraPanel }) })}
        footer={"FOOTER"}
        className={classNames(
            "draggable-panel",
            ExtraPanelSCSS.ExtraPanel,
            extraPanel.isFullScreen && ExtraPanelSCSS.FullScreen,
            !extraPanel.isExpanded && ExtraPanelSCSS.Collapsed,
            !extraPanel.visible && ExtraPanelSCSS.Hidden,
        )}
        resizable={!extraPanel.isFullScreen && extraPanel.isExpanded}
        onResizing={onResizing}
        onResizeEnd={onResizeEnd}
        style={{
            left: extraPanel.panelLocation.x,
            top: extraPanel.panelLocation.y,
            width: extraPanel.panelSize.x,
            height: extraPanel.panelSize.y,
            transform: (translate.x || translate.y)
                ? `translate(${translate.x}px, ${translate.y}px)`
                : undefined,
        }}
    >
        {extraPanel.isExpanded && children}
    </Panel>
}
