export class FileDetailModel {
    id: number;
    name: string;
    key: string;
    // extra properties
    fileTypeId: number;
    fileUploadPercent: number;
    isUploaded: boolean;
    constructor() {
        this.fileUploadPercent = 0;
        this.isUploaded = false;
        this.fileTypeId = 0;
    }
}