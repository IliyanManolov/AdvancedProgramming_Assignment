import React from 'react'
import BannerImage from "../images/banner.jpg"
import "../stylesheets/Banner.css"

function Banner() {
  return (
    <>
      <img src={BannerImage} className="w-screen h-[15vh] object-cover"></img>

      {/* Intentionally NOT centered on the image */}
      <div className="banner-head px-[1vw] text-center text-xl bg-gray-900 bg-opacity-40">

        <div className="text-white text-2xl">
          The best place for movie and show reviews
        </div>
      </div>
    </>
    // <div>
    // </div>
  )
}

export default Banner