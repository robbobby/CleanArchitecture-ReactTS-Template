import type { AxiosInstance } from 'axios';
import axios from 'axios';

/**
 * Service API base class - configures default settings/error handling for inheriting class
 */
export abstract class BaseService {
    protected readonly request: AxiosInstance;

    protected constructor(controller: string, timeout: number = 50000) {
        this.request = axios.create({
            timeout,
            baseURL: `http://localhost:5000/api/${controller}/`
        });
    }
}
