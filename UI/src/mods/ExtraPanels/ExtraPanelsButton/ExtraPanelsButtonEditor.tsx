import { FloatingButton, Panel, Tooltip } from "cs2/ui";
import ExtraPanelsSCSS from "./ExtraPanelsButton.module.scss"
import classNames from "classnames";
import { bindValue, trigger, useValue } from "cs2/api";
import { ExtraPanelType } from "../ExtraPanelType";
import { ExtraPanelsButton } from "./ExtraPanelsButton";
import { ReactElement } from "react";


const ShowExtraPanelsButton$ = bindValue<boolean>("el", "ShowExtraPanelsButton");
const ExtraPanelsMenuOpened$ = bindValue<boolean>("el", 'ExtraPanelsMenuOpened');
const ShouldOpenExtraPanels$ = bindValue<boolean>("el", "ShouldOpenExtraPanels");
const ExtraPanelsList$ = bindValue<ExtraPanelType[]>("el", 'ExtraPanels');


export const ExtraPanelsButtonEditor = (Component: any) => {

    return (props: any) => <div style={{ display:"flex", flexDirection:"row" } }>
        <Component props />
        {ExtraPanelsButton(true)}
    </div>;
}


