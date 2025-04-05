import React, { useState } from 'react';
import FormInput from './FormInput';
import axios from 'axios';

function Register(){
  const [formData, setFormData] = useState({
    username: '',
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
  });

  const [error, setError] = useState('');
  const [userId, setUserId] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setUserId(null);
    setLoading(true);

    try {
      const response = await axios.post('http://localhost:8080/oauth/register', 
        formData,
        {
            headers: {
                'Content-Type': 'application/json',
            }, 
        }
      );

      setUserId(response.data);
    }
    catch (err)
    {
        if (err.response?.status === 400)
        {
            const errorMsg = err.response.data;
            setError(errorMsg);
        }
        else
          setError('An unexpected error occurred.');

    }
    setLoading(false);
    
  };

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white rounded-2xl shadow-lg">
      <h2 className="text-2xl font-bold mb-6 text-center">Register</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <FormInput label="Username" name="username" value={formData.username} onChange={handleChange} required />
        <FormInput label="First Name" name="firstName" value={formData.firstName} onChange={handleChange} />
        <FormInput label="Last Name" name="lastName" value={formData.lastName} onChange={handleChange} />
        <FormInput label="Email" name="email" type="email" value={formData.email} onChange={handleChange} required />
        <FormInput label="Password" name="password" type="password" value={formData.password} onChange={handleChange} required />
        <FormInput label="Confirm Password" name="confirmPassword" type="password" value={formData.confirmPassword} onChange={handleChange} required />

        {/* Conditional rendering (? correct word ?) */}
        {error && <p className="text-red-500">{error}</p>}
        {userId && <p className="text-green-600">Registered successfully!</p>}

        <button
          type="submit"
          className="w-full bg-blue-600 text-white py-2 rounded-xl hover:bg-blue-700 transition"
          disabled={loading}
        >
          {loading ? 'Registering...' : 'Register'}
        </button>
      </form>
    </div>
  );
};

export default Register;
