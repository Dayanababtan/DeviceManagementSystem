db = db.getSiblingDB('DeviceManagementDb');

const collections = ['Devices', 'Users'];

collections.forEach(col => {
    if (!db.getCollectionNames().includes(col)) {
        db.createCollection(col);
        print(`Collection ${col} created.`);
    } else {
        print(`Collection ${col} already exists.`);
    }
});