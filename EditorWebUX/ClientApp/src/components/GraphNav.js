import React, { useState } from 'react';
import { relations } from "./../exampledata";
import { GraphTo } from "./GraphTo";
import { GraphFrom } from "./GraphFrom";


export function GraphNav(initState) {
    console.log(initState);
    const [focus, setFocusName] = useState(initState.children);

    var relTo = [];
    var relFrom = [];

    for (var r of relations) {
        if (r[0] === focus) {
            relFrom.push(r);
        }
        else if (r[2] === focus) {
            relTo.push(r);
        }
    }

    return (
        <div>
            {GraphTo(relTo, setFocusName)}
            <button>
                {focus}
            </button>
            {GraphFrom(relFrom, setFocusName)}
        </div>
    );
}
