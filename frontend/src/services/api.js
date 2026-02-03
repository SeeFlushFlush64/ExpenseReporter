import axios from 'axios';

const API_BASE_URL = 'https://localhost:7005/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Expense API calls
export const expenseApi = {
  // Get all expenses
  getAll: () => api.get('/expense'),
  
  // Get expense by ID
  getById: (id) => api.get(`/expense/${id}`),
  
  // Get expenses by employee
  getByEmployee: (employeeId) => api.get(`/expense/employee/${employeeId}`),
  
  // Get expenses by category
  getByCategory: (categoryId) => api.get(`/expense/category/${categoryId}`),
  
  // Get expenses by status
  getByStatus: (status) => api.get(`/expense/status/${status}`),
  
  // Get expenses by date range
  getByDateRange: (startDate, endDate) => api.get(`/expense/daterange`, {
    params: { startDate, endDate }
  }),
  
  // Create expense
  create: (expenseData) => api.post('/expense', expenseData),
  
  // Update expense status
  updateStatus: (id, status) => api.put(`/expense/${id}/status`, { status }),
  
  // Delete expense
  delete: (id) => api.delete(`/expense/${id}`),
  
  // Get summary by category
  getSummaryByCategory: () => api.get('/expense/summary/category'),
  
  // Get summary by status
  getSummaryByStatus: () => api.get('/expense/summary/status'),
  
  // Export to Excel
  exportToExcel: (startDate, endDate) => 
    api.get('/expense/export/excel', {
      params: { startDate, endDate },
      responseType: 'blob'
    }),
};

// Employee API calls
export const employeeApi = {
  getAll: () => api.get('/employee'),
  getById: (id) => api.get(`/employee/${id}`),
  create: (employeeData) => api.post('/employee', employeeData),
};

// Category API calls
export const categoryApi = {
  getAll: () => api.get('/employee/categories'),
  getById: (id) => api.get(`/employee/categories/${id}`),
  create: (categoryData) => api.post('/employee/categories', categoryData),
};

export default api;