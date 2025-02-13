import { Typed } from "cs2/bindings";
import { Number2 } from "cs2/ui";

export interface ExtraPanelType extends Typed < "" > {
    //id: string;
    icon: string;
    visible: boolean;
    showInSelector: boolean;
    panelLocation: Number2;
    panelSize: Number2;
}