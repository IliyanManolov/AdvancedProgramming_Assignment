import React from 'react'

function NotFound() {
  return (
    <div className="flex flex-col items-center justify-center h-screen bg-gray-100 text-center px-4">
      <h1 className="text-6xl font-bold text-red-500 mb-4">404 Page Not Found</h1>
      <p className="text-xl text-gray-700 mb-6">The page you're looking for doesn't exist.</p>
      <p className="text-xl text-gray-700 mb-6">If you believe this is incorrect please contact our email</p>
    </div>)
}

export default NotFound