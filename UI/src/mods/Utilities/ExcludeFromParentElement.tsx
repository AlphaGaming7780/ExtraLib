import { Modifier } from '@dnd-kit/core';
import { excludeFromBoundingRect } from './ExcludeFromBoudingReact';

export const excludeFromParentElement: Modifier = ({
    containerNodeRect,
    draggingNodeRect,
    transform
}) => {
    if (!draggingNodeRect || !containerNodeRect)
    {
        return transform;
    }

    return excludeFromBoundingRect(transform, draggingNodeRect, containerNodeRect);
}
;