import React from 'react';
import { CSVLink } from "react-csv";

const ExportReactCSV = ({csvData, fileName, headers}) => {

    // const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    // const fileExtension = '.xlsx';

    // const exportToCSV = (csvData, fileName) => {
    //     const ws = XLSX.utils.json_to_sheet(csvData);
    //     const wb = { Sheets: { 'data': ws }, SheetNames: ['data'] };
    //     const excelBuffer = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
    //     const data = new Blob([excelBuffer], {type: fileType});
    //     FileSaver.saveAs(data, fileName + fileExtension);
    // }
    return (
        <CSVLink data={csvData} filename={fileName} headers={headers}>
            <span className="excel-icon" >
                <img src="../assests/images/DashboardIcons/excelN.png" alt="" />
            </span> 
        </CSVLink>
    )
}
 
export default ExportReactCSV;