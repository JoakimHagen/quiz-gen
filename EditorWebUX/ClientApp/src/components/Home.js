import React, { Component } from 'react';
import { questionTemplates, relations, lookup } from "./../exampledata";
import { GraphNav } from './GraphNav';

export class Home extends Component {
  static displayName = Home.name;

  render() {

    for (var r of relations) {
      if (typeof r[0] === "number") {
        r[0] = lookup[r[0]];
      }
      if (typeof r[1] === "number") {
        r[1] = lookup[r[1]];
      }
      if (typeof r[2] === "number") {
        r[2] = lookup[r[2]];
      }
    }

    return (
      <div>
        <GraphNav>Azure Front Door</GraphNav>        
      </div>
    );
    // {GraphNav("Azure Front Door")}  
  }
}

