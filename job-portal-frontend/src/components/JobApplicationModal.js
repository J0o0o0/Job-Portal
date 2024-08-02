import React, { useState } from 'react';
import Modal from 'react-modal';

const JobApplicationModal = ({ isOpen, onRequestClose, jobId }) => {
  const [cvFile, setCvFile] = useState(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    const formData = new FormData();
    formData.append('jobId', jobId);
    formData.append('cvFile', cvFile);

    fetch('https://localhost:7187/api/jobapplication', {
      method: 'POST',
      body: formData,
    })
      .then((response) => response.json())
      .then((data) => {
        console.log('Success:', data);
        onRequestClose(); // Close the modal on success
      })
      .catch((error) => {
        console.error('Error:', error);
      });
  };

  return (
    <Modal isOpen={isOpen} onRequestClose={onRequestClose}>
      <h2>Apply for Job</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Upload CV</label>
          <input
            type="file"
            onChange={(e) => setCvFile(e.target.files[0])}
            required
          />
        </div>
        <button type="submit">Apply</button>
      </form>
    </Modal>
  );
};

export default JobApplicationModal;