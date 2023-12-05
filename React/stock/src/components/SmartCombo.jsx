// import React, {useState} from 'react';
// import { Combobox, ComboboxInput, ComboboxList, ComboboxPopover, ComboboxOption } from '@reach/combobox';

// export const SmartCombo = (props) => {
//     const [searchTerm, setSearchTerm] = useState('');
//     const handleSearchTermChange = (e) => {
//         setSearchTerm(e.target.value);
//     }
//     const data = props.dataFunc(searchTerm);

//     return (
//         <Combobox>
//             <ComboboxInput className="" onChange={handleSearchTermChange}/>
//             {data && (
//             <ComboboxPopover className="shadow-popup">
//                 {data.length > 0 ? (
//                     <ComboboxList>
//                         {data.map((element) => {
//                             return <ComboboxOption key={element.key} value={element.display} />;
//                         })}
//                     </ComboboxList>
//                 ) : (
//                 <span style={{ display: "block", margin: 8 }}>
//                     No results found
//                 </span>
//                 )}
//             </ComboboxPopover>
//             )}
//       </Combobox>
//     );
// }