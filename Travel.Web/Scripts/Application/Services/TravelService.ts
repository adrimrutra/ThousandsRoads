/// <reference path="../_references.ts" />
module Application
{
    export class TravelService extends Service<Travel>
    {
        private $superProvider: IResource;
        constructor($resource: IResourceService)
        {
            super('Travel', $resource);
            this.$superProvider = $resource('Api/Travel/:method/:id', { method: '@method', id: '@Id' },
                {
                    update: { method: 'PUT' }
                });
        }
        PutTraveler(TravelId: number,UserId: number, success: () => void, error: (reasone) => void) {
            this.$superProvider.get({ TravelId: TravelId, UserId: UserId, method: "traveler" }, success, error);
        }
        
    }
} 