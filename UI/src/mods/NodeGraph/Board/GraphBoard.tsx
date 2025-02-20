import classNames from "classnames"
import GraphBoardSCSS from "./GraphBoard.module.scss"
import { MouseSensor, useSensor, useSensors } from '@dnd-kit/core';
import { excludeFromParentElement } from "../../Utilities/ExcludeFromParentElement";
import { ExtraZoom } from "../../ExtraZoom/ExtraZoom";

export interface GraphBoard {
    theme?: any
    children? : any
}

export const GraphBoard = ({ children }: GraphBoard) : JSX.Element => {

    const mouseSensor = useSensor(MouseSensor);
    const sensors = useSensors(mouseSensor);

    return (
        <ExtraZoom
            DndContext={{
                autoScroll:false,
                sensors: sensors,
                modifiers: [excludeFromParentElement]
            }}
        >
            <div
                className={classNames(GraphBoardSCSS.board)}
            >
                Hey
                {children }
            </div>
        </ExtraZoom>
    )
}