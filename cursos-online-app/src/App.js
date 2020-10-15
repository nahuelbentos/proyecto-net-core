import { MuiThemeProvider } from '@material-ui/core';
import React from 'react';
import Login from './components/security/Login';
import ProfileUser from './components/security/ProfileUser';
import RegisterUser from './components/security/RegisterUser';
import theme from './theme/theme';

function App() {
  return (
    <MuiThemeProvider theme={theme}>
      <RegisterUser></RegisterUser>
      {/* <Login></Login> */}
      {/* <ProfileUser></ProfileUser> */}
    </MuiThemeProvider>
  );
}

export default App;
