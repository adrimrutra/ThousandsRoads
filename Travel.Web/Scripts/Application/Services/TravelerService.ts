/// <reference path="../_references.ts" />
module Application {
    export class TravelerService extends Service<Traveler>
    {
        constructor($resource: IResourceService) {
            super('Traveler', $resource);
        }
    }
} 