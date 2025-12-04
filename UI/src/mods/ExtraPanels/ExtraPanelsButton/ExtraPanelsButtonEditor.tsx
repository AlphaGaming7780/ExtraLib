import { bindValue } from "cs2/api";
import React from "react";
import { ExtraPanelType } from "../ExtraPanelType";
import { ExtraPanelsButton } from "./ExtraPanelsButton";


export const ExtraPanelsButtonEditor = (Component: any) => {
    return (props: any) => <div {...props} style={{ display: "flex", flexDirection: "row", alignItems: "center", justifyContent: "center", pointerEvents: "auto" }}>
        <ExtraPanelsButton placement="top" />
        <Component />
    </div>;
}
