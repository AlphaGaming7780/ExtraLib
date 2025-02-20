import type { ClientRect, Data } from '@dnd-kit/core';
import type { Transform } from '@dnd-kit/utilities';

export function excludeFromBoundingRect(
    transform: Transform,
    rect: ClientRect,
    boundingRect: ClientRect
): Transform {
    const value = {
        ...transform,
    };


    if (rect.top + transform.y > boundingRect.top) {
        value.y = boundingRect.top - rect.top;
    } else if (
        rect.bottom + transform.y <
        boundingRect.bottom
    ) {
        value.y = boundingRect.bottom - rect.bottom
    }

    if (rect.left + transform.x > boundingRect.left) {
        value.x = boundingRect.left - rect.left;
    } else if (
        rect.right + transform.x <
        boundingRect.right
    ) {
        value.x = boundingRect.right - rect.right
    }

    return value;
}