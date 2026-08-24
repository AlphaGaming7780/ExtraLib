import { bindValue, useValue } from "cs2/api";
import { useCallback, useRef, useState } from "react";
import { ExtraPanel } from "../ExtraPanel/ExtraPanel";
import { ExtraPanelType } from "../ExtraPanelType";

const path$ = "game-ui/editor/data-binding/editor-tool-bindings.ts"

const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');

export const ExtraPanelsRoot = () => {

    let ExtraPanelsList: ExtraPanelType[] = useValue(ExtraPanelsList$)

    const zCounterRef = useRef(10);
    const [zIndices, setZIndices] = useState<Record<string, number>>({});

    const bringToFront = useCallback((panelId: string) => {
        setZIndices(prev => {
            const currentMax = Math.max(10, ...Object.values(prev));
            if (prev[panelId] === currentMax) return prev;
            zCounterRef.current = currentMax + 1;
            return { ...prev, [panelId]: zCounterRef.current };
        });
    }, []);

    return <div>
        {
            ExtraPanelsList && ExtraPanelsList.length > 0 && ExtraPanelsList.map((extraPanel: ExtraPanelType) => {

                if (!extraPanel.visible) return <></>
                // Content itself (TypedRenderer) is rendered inside ExtraPanel now, not here - see
                // its own comment on effectiveExtraPanel for why (it needs this panel's own live
                // drag/resize preview state, which only exists inside ExtraPanel).
                return <ExtraPanel
                    key={extraPanel.__Type}
                    extraPanel={extraPanel}
                    zIndex={zIndices[extraPanel.__Type] ?? 3}
                    onBringToFront={() => bringToFront(extraPanel.__Type)}
                />
            })
        }
    </div>
}