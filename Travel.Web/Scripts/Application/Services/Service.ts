/// <reference path="../_references.ts" />
module Application
{
    export interface IRouteParams {
        id: string;
        from: string; 
        flat: string; 
        flon: string; 
        fdate: string;
        to: string; 
        tlat: string; 
        tlon: string; 
        tdate: string;
        direction: string; 
        luggage: string; 
        wait: string; 
        early: string; 
        capacity: string; 
        search: string; 
    }

    export interface IResource extends ng.resource.IResourceClass
    {
        update(): IResource;
        update(dataOrParams: any): IResource;
        update(dataOrParams: any, success: Function): IResource;
        update(success: Function, error?: Function): IResource;
        update(params: any, data: any, success?: Function, error?: Function): IResource;       
    }
    export interface IResourceService extends ng.resource.IResourceService
    {
        (url: string, paramDefaults?: any, acctionDescription?: any): IResource;
    }

    export interface IService<T>
    {
        Get(id: number, success: (model: T) => void, error: (reasone) => void): void;
        GetAll(filter: any, success: (models: T[]) => void, error: (reasone) => void): void;
        Create(model: T, success: (model: T) => void, error: (reasone) => void): void;
        Delete(model: T, success: (model: T) => void, error: (reasone) => void): void;
        Update(model: T, success: (model: T) => void, error: (reasone) => void): void;
    }
    export class Service<T> implements IService<T>
    {
        public $provider: IResource;
        constructor(private $controller: string, private $resource: IResourceService)
        {
            this.$provider = this.$resource('Api/:controller/:id', { controller: $controller, id: '@Id' },
                {
                    update:{method:'PUT'}
                }); 
        }
        Get(id: number, success: (model: T) => void, error: (reasone) => void)
        {
            this.$provider.get({ id: id }, success, error);
        }
        GetAll(filter: any, success: (models: T[]) => void, error: (reasone) => void)
        {
            this.$provider.query(filter, success, error)
        }
        Create(model: T, success: (model: T) => void, error: (reasone) => void)
        {
            this.$provider.save(model, success, error);
        }
        Delete(model: T, success: (model: T) => void, error: (reasone) => void)
        {
            this.$provider.delete(null, model, success, error);
        }
        Update(model: T, success: (model: T) => void, error: (reasone) => void)
        {
            this.$provider.update(model, success, error);
        }
    }

} 