import { Panel, Number2, DraggablePanelProps } from "cs2/ui"
import { ExtraPanelType } from "../ExtraPanelType"
import { trigger } from "cs2/api"
import { ExtraPanelHeader } from "./Header/ExtraPanelHeader"
import { FC, MouseEventHandler, useCallback, useEffect, useState } from "react"
import { useMouseDragEvents, DragEventData } from "../../../../game-ui/common/hooks/use-mouse-drag-events"
export function SetPanelPosition(extraPanel : ExtraPanelType, newPos : Number2) {
    trigger("el", "LocationChanged", extraPanel.__Type, newPos)
}

export interface ExtraPanelProps {
    extraPanel: ExtraPanelType,
    children: any
}

function CalculateNewPosition(element : HTMLElement): Number2 {
    var x = element.offsetLeft / (window.innerWidth - element.offsetWidth)
    var y = element.offsetTop / (window.innerHeight - element.offsetHeight)
    return { x,y }
}

export const ExtraPanel = ({ extraPanel, children }: ExtraPanelProps) => {

    const OnResize = (a : any) => { console.log(a) }

    const handleDragEnd = (a: DragEventData) => { SetPanelPosition(extraPanel, CalculateNewPosition(a.currentTarget) ) }
    const { isDragging, handleMouseDown } = useMouseDragEvents({ handleDragEnd: handleDragEnd } );

    //const [value, setValue] = useState(false);
    // will change value to "true" after 5 seconds
    //useEffect(() => { setTimeout(() => setValue(true), 5000) }, []);

    return <Panel
        initialPosition={extraPanel.panelLocation}
        draggable={true}
        onMouseDown={handleMouseDown}
        header={"HEADER"}
        footer={"FOOTER"}
        onEnded={OnResize }
        style={
            {
                //left: extraPanel.panelLocation.x,
                //top: extraPanel.panelLocation.y,
                width: "auto",
                height: "auto",
                visibility: extraPanel.visible ? "visible" : "hidden",
                //transformOrigin: "0% 0% 0px"
            }}
    >
        {children}
        {/*{value && <div> something big here </div>}*/}
    </Panel>
}
