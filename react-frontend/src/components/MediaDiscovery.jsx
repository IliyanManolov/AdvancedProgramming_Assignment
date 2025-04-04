import React from 'react'
import Banner from '../images/banner.jpg'

function MediaDiscovery() {
  return (
    <div>

      <div className="mb-8 font-bold text-4xl text-center">
        New releases
      </div>

      <div className="flex justify-center flex-wrap">

        {/* TODO: Add a function to truncate the name of the movie. Maybe have it pop-up on hover? */}

        {/* TBD: decide if there should be an indication of Movie vs Show in this discovery*/}

        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            FIRST MOVIE
          </div>
        </div>

        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            SECOND MOVIE
          </div>
        </div>

        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            THIRD MOVIE
          </div>
        </div>

        {/* TEMPORARY FOR TESTING */}
        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            FOURTH MOVIE
          </div>
        </div>

        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            FIFTH MOVIE
          </div>
        </div>

        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            SIXTH MOVIE
          </div>
        </div>

        <div className="w-[15vw] h-[25vh] m-5 rounded-lg bg-cover bg-center flex flex-col justify-end" 
          style={{ backgroundImage: `url(${Banner})` }}>
          <div className="text-xl text-white bg-gray-900 bg-opacity-60 p2 text-center w-full h-[4vh] rounded-lg">
            SEVENTH MOVIE
          </div>
        </div>

      </div>

    </div>
  )
}

export default MediaDiscovery