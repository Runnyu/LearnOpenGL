#version 330 core
layout (location = 0) in vec3 aPos;

// 设置顶点大小
// gl_PointSize

// 窗口空间坐标
// gl_FragCoord(vec3)

// 正反面
// gl_FrontFacing(bool)

// 设置深度值
// layout (depth_greater) out float gl_FragDepth;

layout (std140) uniform Matrices
{
    mat4 projection;
    mat4 view;
};
uniform mat4 model;

void main()
{
    gl_Position = projection * view * model * vec4(aPos, 1.0);
}  