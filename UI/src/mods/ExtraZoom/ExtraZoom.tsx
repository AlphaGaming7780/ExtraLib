import { DndContext, useDraggable } from "@dnd-kit/core";
import { Props } from "@dnd-kit/core/dist/components/DndContext/DndContext";
import { Coordinates } from "@dnd-kit/core/dist/types";
import { useValue } from "cs2/api";
import { HTMLProps, useRef, useState, WheelEvent } from "react";

export interface ExtraZoomDraggableElementProps extends ExtraZoomProps {
    coordinates: Coordinates
    SetCoordinates: (coordinates : Coordinates) => void
}

const ExtraZoomDraggableElement = ({ minZoom = 1, maxZoom = 5, scrollVelocity = 0.2, coordinates, SetCoordinates,  children }: ExtraZoomDraggableElementProps) : JSX.Element => {
    const DefaultValue = { x: 0, y: 0 }

    const [zoom, SetZoom] = useState(1)
    //const ref = useRef<HTMLDivElement>(null)

    const { attributes, listeners, setNodeRef, transform, isDragging, node } = useDraggable({
        id: 'ExtraZoom'
    });

    var translate: Coordinates = { x: 0, y: 0 }
    translate.x = transform ? transform.x : 0
    translate.y = transform ? transform.y : 0

    const getNewPosition = (clientX: number, clientY: number, newZoom: number) => {
        if (newZoom === 1 || !node.current) return DefaultValue

        if (newZoom > zoom) {
            // Get container coordinates
            const rect = node.current.getBoundingClientRect()

            // Retrieve rectangle dimensions and mouse position
            const [centerX, centerY] = [rect.width / 2, rect.height / 2]
            const [relativeX, relativeY] = [clientX - rect.left - window.scrollX, clientY - rect.top - window.scrollY]

            // If we are zooming down, we must try to center to mouse position
            const [absX, absY] = [(centerX - relativeX) / zoom, (centerY - relativeY) / zoom]
            const ratio = newZoom - zoom
            return { x: coordinates.x + absX * ratio, y: coordinates.y + absY * ratio }
        } else {
            // If we are zooming down, we shall re-center the element
            return { x: (coordinates.x * (newZoom - 1)) / (zoom - 1), y: (coordinates.y * (newZoom - 1)) / (zoom - 1) }
        }
    }

    const handleMouseWheel = (event: WheelEvent<HTMLDivElement>) => {
        event.preventDefault();

        if (!node.current) return;

        // Use the scroll event delta to determine the zoom velocity
        var velocity = (-event.deltaY * scrollVelocity) / 100;
        // Set the new zoom level
        var newZoom = Math.max(Math.min(zoom + velocity, maxZoom), minZoom);
        var newPosition = coordinates;

        if (newZoom !== zoom) {
            newPosition = newZoom !== minZoom ? getNewPosition(event.clientX, event.clientY, newZoom) : DefaultValue;
        }
        SetZoom(newZoom);
        SetCoordinates(newPosition);
    }

    return (
        <div
            ref={setNodeRef}

            {...listeners}
            {...attributes}

            onWheel={handleMouseWheel}

            style={{
                position: "relative",
                left: `${coordinates.x}px`,
                top : `${coordinates.y}px`,
                transform: `translate(${translate.x}px, ${translate.y}px) scale(${zoom})`
            }}>

            {children }


        </div>
    )
}


export interface ExtraZoomProps  {
    children: React.ReactNode
    minZoom?: number;
    maxZoom?: number;
    scrollVelocity?: number;
    DndContext?: Props 
}

export const ExtraZoom = ( props: ExtraZoomProps ): JSX.Element => {

    const [coordinates, SetCoordinates] = useState<Coordinates>({ x: 0, y: 0 });

    return (
        <DndContext
            {...props.DndContext}

            onDragEnd={({ delta }) => {
                SetCoordinates(({ x, y }) => {
                    return {
                        x: x + delta.x,
                        y: y + delta.y,
                    };
                });
            }}

        >
            <div style={{ overflow: "auto" }}>
                <ExtraZoomDraggableElement
                    {...props}
                    coordinates={coordinates}
                    SetCoordinates={SetCoordinates }
                />
            </div>
        </DndContext>
    )
}