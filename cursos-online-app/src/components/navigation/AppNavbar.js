import { AppBar } from '@material-ui/core';
import React from 'react';
import BarSession from './bar/BarSession';

const AppNavbar = () => {
  return (
    <AppBar position="static">
      <BarSession></BarSession>
    </AppBar>
  );
};

export default AppNavbar;
