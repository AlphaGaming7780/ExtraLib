import { HTMLAttributes, MouseEventHandler, ReactNode } from "react";
import { CloseExtraPanel, CollapseExtraPanel, ExpandExtraPanel, ExtraPanelType, SetFullScreenExtraPanel } from "../../ExtraPanelType";
import classNames from "classnames";
import ExtraPanelHeaderSCSS from "./ExtraPanelHeader.module.scss"
import { useLocalization } from "cs2/l10n";
import { IconButton } from "../../../../../game-ui/common/input/button/icon-button";
import { PanelSCSS } from "../../../../../game-ui/common/panel/panel.module.scss";
import { RoundHighlightButtonSCSS } from "../../../../../game-ui/common/input/button/themes/round-highlight-button.module.scss";
import { FOCUS_DISABLED$ } from "../../../../../game-ui/common/focus/focus-key";

export interface propsExtraPanelHeader extends HTMLAttributes<HTMLDivElement> {
    extraPanel: ExtraPanelType;
}


export const ExtraPanelHeader = ({ extraPanel, onMouseUp }: propsExtraPanelHeader): ReactNode => {

    const { translate } = useLocalization();

    return <div className={classNames( PanelSCSS.titleBar)} onMouseUp={onMouseUp}>
        
        <img src={extraPanel.icon} className={classNames( PanelSCSS.icon)} />

        <div className={classNames(PanelSCSS.iconSpace)} />

        {
            extraPanel.canFullScreen && <div className={classNames(PanelSCSS.iconSpace)} />
        }  

        <div className={classNames( PanelSCSS.title) }>
            {translate(`ExtraPanelHeaderName[${extraPanel.__Type}]`, `ExtraPanelHeaderName[${extraPanel.__Type}]`) }
        </div>

        <IconButton
            src={ extraPanel.isExpanded ? PanelSCSS.toggleIconExpanded : PanelSCSS.toggleIcon}
            className={classNames( PanelSCSS.closeButton)}
            tinted={true}
            focusKey={FOCUS_DISABLED$}
            theme={RoundHighlightButtonSCSS}
            onSelect={(e: any) => {
                extraPanel.isExpanded ?
                    CollapseExtraPanel(extraPanel) :
                    ExpandExtraPanel(extraPanel)
            }}
            onMouseDown={(e: MouseEvent) => {
                e.preventDefault();
                e.stopPropagation();
            }}
        />

        {
            extraPanel.canFullScreen &&
            <IconButton
                src={extraPanel.isFullScreen ? "/Media/Glyphs/ThickStrokeArrowUp.svg" : PanelSCSS.toggleIcon}
                className={classNames(PanelSCSS.closeButton)}
                tinted={true}
                focusKey={FOCUS_DISABLED$}
                theme={RoundHighlightButtonSCSS}
                onSelect={(e: any) => {
                    SetFullScreenExtraPanel(extraPanel, !extraPanel.isFullScreen)
                }}
                onMouseDown={(e: MouseEvent) => {
                    e.preventDefault();
                    e.stopPropagation();
                }}
            />
        }

        <IconButton
            src={PanelSCSS.closeIcon}
            className={classNames( PanelSCSS.closeButton)}
            tinted={true}
            focusKey={FOCUS_DISABLED$}
            theme={RoundHighlightButtonSCSS}
            onSelect={ () => CloseExtraPanel(extraPanel)}
            onMouseDown={(e : MouseEvent) => {
                e.preventDefault();
                e.stopPropagation();
            }}
        />

    </div>


}