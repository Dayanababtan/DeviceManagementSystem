db = db.getSiblingDB('DeviceManagementDb');

const mobileDevices = [
    {
        Name: "iPhone 15 Pro",
        Manufacturer: "Apple",
        Type: "Phone",
        OS: "iOS",
        OSVersion: "17.4",
        Processor: "A17 Pro",
        RAMAmount: "8GB",
        Description: "Corporate executive device",
        UserId: null
    },
    {
        Name: "Galaxy S24 Ultra",
        Manufacturer: "Samsung",
        Type: "Phone",
        OS: "Android",
        OSVersion: "14.0",
        Processor: "Snapdragon 8 Gen 3",
        RAMAmount: "12GB",
        Description: "Testing device for Android builds",
        UserId: null
    },
    {
        Name: "iPad Pro 12.9",
        Manufacturer: "Apple",
        Type: "Tablet",
        OS: "iPadOS",
        OSVersion: "17.2",
        Processor: "M2",
        RAMAmount: "16GB",
        Description: "Design team tablet",
        UserId: null
    },
    {
        Name: "Pixel Tablet",
        Manufacturer: "Google",
        Type: "Tablet",
        OS: "Android",
        OSVersion: "14.0",
        Processor: "Tensor G2",
        RAMAmount: "8GB",
        Description: "Standard issue office tablet",
        UserId: null
    }
];

var users = [
    {
        Name: "Alice Johnson",
        Role: "Admin",
        Location: "New York"
    },
    {
        Name: "Bob Smith",
        Role: "Developer",
        Location: "London"
    },
    {
        Name: "Charlie Davis",
        Role: "Manager",
        Location: "Remote"
    }
];

if (db.Devices.countDocuments({}) === 0) {
    db.Devices.insertMany(mobileDevices);
    print('Successfully seeded ' + mobileDevices.length + ' devices.');
} else {
    print("Devices already exist in DB. Skipping seed to prevent duplicates.");
}

if (db.Users.countDocuments({}) === 0) {
    db.Users.insertMany(users);
    print('Successfully seeded ' + users.length + ' users.');
} else {
    print('Users collection already has data. Skipping seed to prevent duplicates.');
}