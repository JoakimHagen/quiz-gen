import React from 'react';
import { Container } from 'reactstrap';
import { Route } from 'react-router';

import { Home } from './components/Home';
import { NavMenu } from './components/NavMenu';

import './custom.css'

export function app() {
  return (
    <div>
      <NavMenu />
      <Container>
        <Route exact path='/' component={Home} />
      </Container>
    </div>
  );
}

export default app;