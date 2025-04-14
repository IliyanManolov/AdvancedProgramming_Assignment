import './App.css';
import NavigationBar from './components/NavigationBar';
import Banner from './components/Banner';
import MediaDiscovery from './components/MediaDiscovery';
import Register from './components/Register';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Movies from './components/Movies';
import NotFound from './components/shared/NotFound';
import Login from './components/Login';
import MediaDetails from './components/MediaDetails';
import Shows from './components/Shows';
import AdminPanel from './components/admin/AdminPanel';
import Watchlist from './components/Watchlist';

// Tailwind will be used for CSS

// TODO: add single media page with more details (director, actors, etc.)
// ^ initial version made, needs maaaajor UI overhauling # I am a backend dev dont shoot me
// TODO: Change front page - display only 6 (or maybe 10?) latest movies by date added (new endpoint)

// ADD PROTECTIONS FOR ACCESSING ADMIN PANEL

function App() {
  return (

    <>
      <BrowserRouter>

        <NavigationBar></NavigationBar>
        <Routes>
          <Route path="/" element={
            <>

              <Banner>
              </Banner>

              <MediaDiscovery>
              </MediaDiscovery>
            </>
          }></Route>

          <Route path="/register" element={
            <>
              <Banner></Banner>
              <Register></Register>
            </>
          }>
          </Route>

          <Route path="/login" element={
            <>
              <Banner></Banner>
              <Login></Login>
            </>
          }>
          </Route>

          <Route path="/movies" element={
            <>
              <Movies></Movies>
            </>
          }></Route>

          <Route path="/movies/:id" element={
            <>
              <MediaDetails></MediaDetails>
            </>
          }></Route>

          <Route path="/shows" element={
            <>
              <Shows></Shows>
            </>
          }></Route>

          <Route path="/shows/:id" element={
            <>
              <MediaDetails></MediaDetails>
            </>
          }></Route>

          <Route path="/watchlist" element={
            <>
              <Watchlist></Watchlist>
            </>
          }></Route>

          <Route path="/adminpanel" element={
            <>
              <AdminPanel></AdminPanel>
            </>
          }></Route>

          <Route path="*" element={
            <NotFound></NotFound>
          }></Route>

        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
