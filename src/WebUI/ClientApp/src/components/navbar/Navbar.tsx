import { useIsLoggedIn } from '../../hooks';
import { Routes as routes } from '../../config';
import { generatePath, NavLink } from 'react-router-dom';

import type { FunctionComponent } from 'react';

const Navbar: FunctionComponent = () => {
    const isLoggedIn = useIsLoggedIn();

    return (
        <nav
            role='navigation'
            className='navbar'
            aria-label='main navigation'
        >
            <div className='navbar-wrapper'>
                <div className='brand-wrapper'>
                    <h1>ConsoleApi</h1>
                </div>
                <div className='navbar-routes'>
                    {isLoggedIn &&
                        routes
                            .filter((x) => x.showInNav)
                            .map(({path, name, params}) => (
                                <NavLink
                                    key={name}
                                    to={generatePath(path, params)}
                                    className={({isActive}) => 'navbar-item' + (isActive ? ' is-active' : '')}
                                >
                                    {name}
                                </NavLink>
                            ))}
                </div>
            </div>
        </nav>
    );
};
export default Navbar;
