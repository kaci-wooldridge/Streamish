import React from "react";
import { Link } from "react-router-dom";
import { logout } from "../modules/authManager";

const Header = () => {
    return (
        <nav className="navbar navbar-expand navbar-dark">
            <Link to="/" className="navbar-brand">
                StreamISH
            </Link>
            <ul className="navbar-nav mr-auto">
                <div className="nav-links">
                <li className="nav-item">
                    <Link to="/" className="nav-link">
                        Feed
                    </Link>
                </li>
                <li className="nav-item">
                    <Link to="/videos/add" className="nav-link">
                        New Video
                    </Link>
                </li>
                <li className="nav-item">
                    <Link to="/" className="nav-link" onClick={logout}>
                        Logout
                    </Link>
                    </li>
                </div>
            </ul>
        </nav>
    );
};

export default Header;