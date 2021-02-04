import React from 'react';

export function GraphTo(relTo, setFocusName) {

    var cells = relTo.flatMap(relation => [
      <input type="text" key={relation[0] + relation[1]} defaultValue={relation[0]} />,
      <input type="text" key={relation[0] + relation[1] +"rel"} defaultValue={relation[1]} />,
      <button key={relation[0] + relation[1] +"button"} onClick={() => setFocusName(relation[0])}>Go</button>]
      );

    return (
        <div className="grid">
          {cells}
        </div>
    );
}
