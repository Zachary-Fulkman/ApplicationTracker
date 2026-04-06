/**
 * Represents a single job application in the system.
 */
export interface Application {
    id: number;
    companyName: string;
    dateApplied: string;
    status: string;
    notes: string;
}

/**
 * Payload used when registering a new user.
 */
export interface RegisterUserRequest {
    email: string;
    password: string;
}

/**
 * Payload used when logging in a user.
 */
export interface LoginUserRequest {
    email: string;
    password: string;
}

/**
 * Response returned after a successful login.
 */
export interface LoginResponse {
    token: string;
}

/**
 * Generic structure for paginated API responses.
 */
export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
}

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

function getAuthHeaders(): HeadersInit {
    const token = localStorage.getItem("token");

    if (!token) {
        return {
            "Content-Type": "application/json",
        };
    }

    return {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
    };
}

/**
 * Query parameters used when fetching applications (filters + pagination).
 */
export interface GetApplicationsParams {
    status?: string;
    company?: string;
    page?: number;
    pageSize?: number;
}

/**
 * Fetches applications from the backend with optional filters and pagination.
 */
export async function getApplications(
    params?: GetApplicationsParams
): Promise<PagedResult<Application>> {
    const url = new URL(BASE_URL);

    if (params?.status) {
        url.searchParams.append("status", params.status);
    }

    if (params?.company) {
        url.searchParams.append("company", params.company);
    }

    if (params?.page) {
        url.searchParams.append("page", params.page.toString());
    }

    if (params?.pageSize) {
        url.searchParams.append("pageSize", params.pageSize.toString());
    }

    const response = await fetch(url.toString(), {
        headers: getAuthHeaders(),
    });

    if (!response.ok) {
        throw new Error("Failed to fetch applications");
    }

    return response.json();
}

/**
 * Payload used when creating a new job application.
 */
export interface CreateApplicationRequest {
    companyName: string;
    dateApplied: string;
    status: string;
    notes: string;
}

/**
 * Sends a request to create a new application.
 */
export async function createApplication(data: CreateApplicationRequest) {
    const response = await fetch(BASE_URL, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        throw new Error("Failed to create application");
    }

    return response.json();
}

/**
 * Deletes an application by its ID.
 */
export async function deleteApplication(id: number) {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: "DELETE",
        headers: getAuthHeaders(),
    });

    if (!response.ok) {
        throw new Error("Failed to delete application");
    }
}

/**
 * Payload used when updating an existing application.
 */
export interface UpdateApplicationRequest {
    companyName: string;
    dateApplied: string;
    status: string;
    notes: string;
}

/**
 * Updates an existing application by ID.
 */
export async function updateApplication(
    id: number,
    data: UpdateApplicationRequest
) {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: "PUT",
        headers: getAuthHeaders(),
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        throw new Error("Failed to update application");
    }
}

/**
 * Sends a request to register a new user account.
 */
export async function registerUser(data: RegisterUserRequest) {
    const response = await fetch(`${BASE_URL.replace("/application", "/auth")}/register`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        throw new Error("Failed to register user");
    }

    return response.json();
}

/**
 * Sends a request to log in a user and returns a JWT token.
 */
export async function loginUser(data: LoginUserRequest): Promise<LoginResponse> {
    const response = await fetch(`${BASE_URL.replace("/application", "/auth")}/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
    });

    if (!response.ok) {
        throw new Error("Failed to log in");
    }

    return response.json();
}