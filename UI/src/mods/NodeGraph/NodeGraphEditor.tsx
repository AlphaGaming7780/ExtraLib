import { DndContext } from "@dnd-kit/core";
import { Component } from "react";
import { GraphBoard } from "./Board/GraphBoard";

export interface NodeGraphEditorPorps {

}

export const NodeGraphEditor = (props: NodeGraphEditorPorps) : JSX.Element => {
    return (
        //<div style={{ width: "auot", height: "auto"}} >
            <GraphBoard>
                YES
            </GraphBoard>
        //</div>
    )
}