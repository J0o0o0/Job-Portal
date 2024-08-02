import React, { useState, useEffect } from 'react';
import ApplyJob from './ApplyJob';

const JobList = () => {
  const [jobs, setJobs] = useState([]);
  const [selectedJob, setSelectedJob] = useState(null);

  useEffect(() => {
    fetch('https://localhost:7187/api/job')
      .then(response => response.json())
      .then(data => setJobs(data))
      .catch(error => console.error('Error fetching jobs:', error));
  }, []);

  const handleApplyClick = (job) => {
    setSelectedJob(job);
  };

  const closePopup = () => {
    setSelectedJob(null);
  };

  return (
    <div>
      <h2>Job List</h2>
      <ul>
        {jobs.map((job) => (
          <li key={job.id}>
            {job.jobTitle}
            <button onClick={() => handleApplyClick(job)}>Apply</button>
          </li>
        ))}
      </ul>
      {selectedJob && (
        <ApplyJob job={selectedJob} onClose={closePopup} />
      )}
    </div>
  );
};

export default JobList;