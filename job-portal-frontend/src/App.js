import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavBar from './components/NavBar';
import Login from './components/Login';
import Register from './components/Register';
import JobList from './components/JobList';
import EditJob from './components/EditJob';
import EditProfile from './components/EditProfile';
import ApplyJob from './components/ApplyJob';
import ViewApplications from './components/ViewApplications';

function App() {
  return (
    <Router>
      <NavBar/>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/jobs" element={<JobList />} />
        <Route path="/edit-job/:jobId" element={<EditJob />} />
        <Route path="/edit-profile" element={<EditProfile />} />
        <Route path="ApplyJob" element={<ApplyJob />} />
        <Route path="/applications" element={<ViewApplications />} />
        <Route path="/" element={<JobList />} />
      </Routes>
    </Router>
  );
}

export default App;