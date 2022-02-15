export interface ErrorResponse {
    status: number;
    title?:string;
    errors?:string | ValidationErrorResponse;

}
export interface ValidationErrorResponse {
	errors: ValidationErrorPayload;
}
export interface ValidationErrorPayload {
    [key:string]:string[]
}
