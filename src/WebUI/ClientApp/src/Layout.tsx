import type { FunctionComponent } from 'react';
import { Fragment } from 'react';
import { Footer, Settings } from './components';
import Navbar from "./components/navbar/Navbar";

const Layout: FunctionComponent = ({children}) => (
    <Fragment>
        <Navbar/>
        <Settings/>
        {children}
        <Footer/>
    </Fragment>
);

export default Layout;