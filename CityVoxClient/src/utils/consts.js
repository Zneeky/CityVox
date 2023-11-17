export const CLOUDINARY_UPLOAD_PRESET = "z1rbueyh";
export const CLOUDINARY_UPLOAD_URL =
    "https://api.cloudinary.com/v1_1/dtu8pzhll/image/upload";

export const httpsApiCode = "https://localhost:7057/";

export function getCurrentPosition(options = {}) {
    return new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resolve, reject, options);
    });
}

export const ReportTypes = 
[{name:"Littering", value: 0}, 
 {name:"Graffiti", value: 1}, 
 {name:"Road issues", value: 2}, 
 {name:"Streetlight issues", value: 3}, 
 {name:"Parking violation", value: 4},
 {name:"Public facilities", value: 5},
 {name:"Tree hazards", value: 6},
 {name:"Traffic concerns", value: 7},
 {name:"Wildlife", value: 8},
 {name:"Public transport", value: 9},
 {name:"Sidewalks", value: 10},
 {name:"Other", value: 11},
]

export const ReportStatusTypes = 
[{name:"Reported", value: 0}, 
 {name:"Approved", value: 1}, 
 {name:"In progress", value: 2}, 
 {name:"Resolved", value: 3}, 
 {name:"Rejected", value: 4},
]

export const EmergencyTypes =
[{name:"Fire", value: 0}, 
 {name:"Medical", value: 1}, 
 {name:"Police", value: 2},
 {name:"Traffic Accident", value: 3}, 
 {name:"Public Disturbance", value: 4}, 
 {name:"Hazardous Materials", value: 5},
 {name:"Other", value: 6},
]

export const EmergencyStatusTypes = 
[{name:"Reported", value: 0}, 
 {name:"FalseAlarm", value: 1}, 
 {name:"Acknowledged", value: 2},
 {name:"Escalated", value: 3}, 
 {name:"UnderControl", value: 4}, 
 {name:"Resolved", value: 5},
]

export const InfIssueTypes = 
[{name:"Sewer and water", value: 0}, 
 {name:"Electricity", value: 1}, 
 {name:"Gas", value: 2}, 
 {name:"Heating", value: 3}, 
 {name:"Telecommunications", value: 4},
 {name:"Building", value: 5},
 {name:"Other", value: 6},
]

export const InfIssueStatusTypes = 
[{name:"Reported", value: 0}, 
 {name:"Acknowledged", value: 1}, 
 {name:"Repair inprogress", value: 2}, 
 {name:"Partially resolved", value: 3}, 
 {name:"Resolved", value: 4},
 {name:"Closed unresolved", value: 5},
]
