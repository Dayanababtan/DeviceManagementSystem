db = db.getSiblingDB('DeviceManagementDb');

const mobileDevices = [
    {
        deviceId: 1,
        name: "iPhone 15 Pro",
        manufacturer: "Apple",
        type: "SmartPhone",
        os: "iOS",
        osVersion: "17.4",
        processor: "A17 Pro",
        ramAmount: "8GB",
        description: "Corporate executive device",
        userId: 2,
    },
    {
        deviceId: 2,
        name: "Galaxy S24 Ultra",
        manufacturer: "Samsung",
        type: "SmartPhone",
        os: "Android",
        osVersion: "14.0",
        processor: "Snapdragon 8 Gen 3",
        ramAmount: "12GB",
        description: "Testing device for Android builds",
        userId: 1,
    },
    {
        deviceId: 3,
        name: "iPad Pro 12.9",
        manufacturer: "Apple",
        type: "Tablet",
        os: "iPadOS",
        osVersion: "17.2",
        processor: "M2",
        ramAmount: "16GB",
        description: "Design team tablet",
        userId: 2,
    },
    {
        deviceId: 4,
        name: "Pixel Tablet",
        manufacturer: "Google",
        type: "Tablet",
        os: "Android",
        osVersion: "14.0",
        processor: "Tensor G2",
        ramAmount: "8GB",
        description: "Standard issue office tablet",
        userId: 3,
    }
];

var users = [
    {
        userId: 1,
        name: "Alice Johnson",
        role: "Admin",
        location: "New York",
        email: "alice@test.com",   
        password: "$2a$10$8K1p/a0dL1LXJ17.B9Vv7uY.8G0H2I6yGZzH5KxG.VzHjGzHjGzHj"// This is a real bcrypt hash for "password123"

    },
    {
        userId: 2,
        name: "Bob Smith",
        role: "Developer",
        location: "London",
        email: "bob@test.com",          
        password: "$2a$10$8K1p/a0dL1LXJ17.B9Vv7uY.8G0H2I6yGZzH5KxG.VzHjGzHjGzHj"// This is a real bcrypt hash for "password123"
    },
    {
        userId: 3,
        name: "Charlie Davis",
        role: "Manager",
        location: "Remote",
        email: "charlie@test.com", 
        password: "$2a$10$8K1p/a0dL1LXJ17.B9Vv7uY.8G0H2I6yGZzH5KxG.VzHjGzHjGzHj" // This is a real bcrypt hash for "password123"
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

db.Counters.updateOne({ _id: "userId" }, { $set: { Value: 3 } }, { upsert: true });
db.Counters.updateOne({ _id: "deviceId" }, { $set: { Value: 4 } }, { upsert: true });