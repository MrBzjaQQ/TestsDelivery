import React, {ReactNode} from 'react';
import './centerContainer.scss';

export interface CenterContainerProps {
    children: ReactNode
}

const CenterContainer = (props: CenterContainerProps) => {
    return <div className="center-container">
        {props.children}
    </div>;
};

export default CenterContainer;