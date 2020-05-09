#ifndef _MESH_H_
#define _MESH_H_

#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

#include <string>
#include <vector>

#include "shader_s.h"

using namespace std;

struct Vertex {
	glm::vec3 Position;
	glm::vec3 Normal;
	glm::vec2 TexCoords;
};

struct Texture {
	unsigned int id;
	string type;
	string path;  // 我们储存纹理的路径用于与其它纹理进行比较
};

class Mesh {
public:
	/*  网格数据  */
	vector<Vertex> vertices;
	vector<unsigned int> indices;
	vector<Texture> textures;
	/*  函数  */
	Mesh(vector<Vertex> vertices, vector<unsigned int> indices, vector<Texture> textures);
	void Draw(Shader shader);
private:
	/*  渲染数据  */
	unsigned int VAO, VBO, EBO;
	/*  函数  */
	void setupMesh();
};

#endif