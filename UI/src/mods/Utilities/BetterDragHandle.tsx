import { JSXElementConstructor, ReactElement, useRef, useState } from "react"
import { DragEventData, useMouseDragEvents } from "../../../game-ui/common/hooks/use-mouse-drag-events"
import { Number2 } from "cs2/bindings"
import React from "react"

export interface BetterDragEventData {
    x: number,
    y: number,
    startX: number
    startY: number
}
export interface BetterDragHandleProps {
    children: any,
    onDragStart?: (e: BetterDragEventData) => boolean
    onDrag: (e: BetterDragEventData) => void
    onDragEnd?: (e: BetterDragEventData) => void
}

export const BetterDragHandle = ( { children, onDragStart, onDrag, onDragEnd } : BetterDragHandleProps ) : JSX.Element => {

    const dragOffset = useRef<Number2>({ x: 0, y: 0 });
    const isDragging = useRef<boolean>(false);

    const handleDragStart = (e: MouseEvent): boolean => {

        dragOffset.current = {
            x: e.clientX,
            y: e.clientY
        };

        var k = e.clientX;
        var j = e.clientY;
        var g = dragOffset.current;

        isDragging.current = true;

        if (onDragStart) return onDragStart({ x: e.clientX, y: e.clientY, startX: dragOffset.current.x, startY: dragOffset.current.y });
        else onDrag({ x: e.clientX, y: e.clientY, startX: dragOffset.current.x, startY: dragOffset.current.y })
        return true ;
    }

    const handleDragging = (e: DragEventData): void => {
        if(isDragging.current) onDrag({ x: e.clientX, y: e.clientY, startX: dragOffset.current.x, startY: dragOffset.current.y });
        return
    }

    const handleDragEnd = (e: DragEventData): void => {
        isDragging.current = false;
        if (onDragEnd) onDragEnd({ x: e.clientX, y: e.clientY, startX: dragOffset.current.x, startY: dragOffset.current.y });
        else onDrag({ x: e.clientX, y: e.clientY, startX: dragOffset.current.x, startY: dragOffset.current.y })
        return 
    }

    const { handleMouseDown } = useMouseDragEvents({ handleDragStart: handleDragStart, handleDragging: handleDragging, handleDragEnd: handleDragEnd  } )

    return <>
        {React.Children.map(children, (e) => {
            return React.isValidElement(e) ? 
                React.cloneElement(e as any, { onMouseDown: handleMouseDown }) :
                <div onMouseDown={handleMouseDown } >
                    { e }
                </div>
        })}
    </>

}