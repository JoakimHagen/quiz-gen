import React from 'react';

export function GraphFrom(relFrom, setFocusName) {

    var cells = relFrom.flatMap(relation => [
      <input type="text" key={relation[1] + relation[2]} defaultValue={relation[1]} />,
      <input type="text" key={relation[1] + relation[2] +"rel"} defaultValue={relation[2]} />,
      <button key={relation[1] + relation[2] +"button"} onClick={() => setFocusName(relation[2])}>Go</button>]
      );

    return (
        <div className="grid">
          {cells}
        </div>
    );
}
