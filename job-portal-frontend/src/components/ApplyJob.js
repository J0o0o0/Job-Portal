import React, { useState } from 'react';

const ApplyJob = ({ job, onClose }) => {
  const [cvFile, setCvFile] = useState(null);
  const [filePath, setFilePath] = useState('');

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      // Simulate file upload and obtain file path
      uploadFile(file).then(path => setFilePath(path));
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = {
      jobId: job.id,
      cvFilePath: filePath
    };

    fetch('https://localhost:7187/api/jobapplication', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(formData)
    })
    .then(response => response.json())
    .then(data => {
      console.log('Success:', data);
      onClose(); // Close popup on successful submission
    })
    .catch(error => console.error('Error:', error));
  };

  const uploadFile = async (file) => {
    // Mock function to upload file and get a file path/identifier
    // Replace with actual file upload logic
    return new Promise((resolve) => {
      setTimeout(() => resolve(`path/to/uploaded/${file.name}`), 1000); // Mock path
    });
  };

  return (
    <div className="popup">
      <div className="popup-content">
        <h3>Apply for {job.jobTitle}</h3>
        <form onSubmit={handleSubmit}>
          <div>
            <label>Upload CV</label>
            <input type="file" onChange={handleFileChange} required />
            {filePath && <p>File Path: {filePath}</p>}
          </div>
          <button type="submit">Submit Application</button>
          <button type="button" onClick={onClose}>Close</button>
        </form>
      </div>
    </div>
  );
};

export default ApplyJob;