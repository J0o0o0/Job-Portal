import React from 'react';
import { Link } from 'react-router-dom';
import '../style/NavBar.css';

const NavBar = () => {
    return (
        <nav>
            <ul>
                <li>
                    <Link to="/">Home</Link>
                </li>
                <li>
                    <Link to="/login">Login</Link>
                </li>
                <li>
                    <Link to="/login">Logout</Link>
                </li>
                <li>
                    <Link to="/register">Register</Link>
                </li>
                <li>
                    <Link to="/jobs">View All Jobs</Link>
                </li>
                <li>
                    <Link to="/edit-job">Edit Job</Link>
                </li>
                <li>
                    <Link to="/edit-profile">Edit Profile</Link>
                </li>
                <li>
                    <Link to="/ApplyJob">Apply for a Job</Link>
                </li>
                <li>
                    <Link to="/applications">View All Applications</Link>
                </li>
            </ul>
        </nav>
    );
};

export default NavBar;