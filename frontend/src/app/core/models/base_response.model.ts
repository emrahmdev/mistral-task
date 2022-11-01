export type BaseResponse<T> = {
    status: boolean;
    data: T | string;
}