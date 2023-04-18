import { onLoginStatusChange, me } from "./modules/authManager";
import { BrowserRouter as Router } from "react-router-dom";
import "./App.css";
import ApplicationViews from "./components/ApplicationViews";
import Header from "./components/Header";
import React, { useEffect, useState } from "react";
import { Spinner } from "reactstrap";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(null);
  const [userProfile, setUserProfile] = useState(null);

  useEffect(() => {
    onLoginStatusChange(setIsLoggedIn);
  }, []);

  useEffect(() => {
    if (isLoggedIn) {
      me().then(setUserProfile);
    } else {
      setUserProfile(null);
    }
  }, [isLoggedIn]);

  // The "isLoggedIn" state variable will be null until //  the app's connection to firebase has been established.
  //  Then it will be set to true or false by the "onLoginStatusChange" function
  if (isLoggedIn === null) {
    // Until we know whether or not the user is logged in or not, just show a spinner
    return <Spinner className="app-spinner dark" />;
  }

  return (
    <div className="App">
      <Router>
        <Header isLoggedIn={isLoggedIn} userProfile={userProfile} />
        <ApplicationViews isLoggedIn={isLoggedIn} />
      </Router>
    </div>
  );
}

export default App;
